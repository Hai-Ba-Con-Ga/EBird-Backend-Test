using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Bird;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using EBird.Infrastructure.Context;
using EBird.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EBird.Test.System.Services;

[TestFixture]
public class BirdServicesTest : IDisposable
{
    protected readonly ApplicationDbContext _context;
    private readonly IBirdService _birdServices;
    private IWapperRepository _wapperRepository;
    private IMapper _mapper;
    private IUnitOfValidation _validator;

    private Guid AccountId { get; set; }
    private Guid BirdTypeId { get; set; }
    private Guid BirdId { get; set; }
    private BirdRequestDTO[] BirdRequest { get; set; }


    public BirdServicesTest()
    {
        //In memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "EBird")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        //Repository
        _wapperRepository = new WapperRepository(_context);
        //Validation 
        _validator = new UnitOfValidation(_wapperRepository);
        //AutoMapper
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BirdEntity, BirdCreateDTO>().ReverseMap();
            cfg.CreateMap<BirdEntity, BirdRequestDTO>().ReverseMap();
        });
        _mapper = configuration.CreateMapper();
        //Service
        _birdServices = new BirdService(_wapperRepository, _mapper, _validator, null);

        //Create testing account 
        AccountEntity account = new AccountEntity()
        {
            FirstName = "Test",
            LastName = "Test",
            Username = "Test",
            Email = "Test",
            Role = RoleAccount.User,
            Description = "Test",
            Password = "test"
        };
        _wapperRepository.Account.CreateAsync(account);
        AccountId = account.Id;

        //Create birdType
        BirdTypeEntity birdType = new BirdTypeEntity()
        {
            TypeName = "Test",
            TypeCode = "TEST",
            CreatedDatetime = DateTime.Now
        };
        _wapperRepository.BirdType.CreateAsync(birdType);
        BirdTypeId = birdType.Id;

        //Get wrong argument list
        var list = MockData.BirdMockData.GetWrongArgBirdCreateDTOs();

        if(list.Count == 0)
            throw new Exception("Can get data");
        _wrongArgBirdCreateDTOList.AddRange(list);

        //Get right argument list
        list = MockData.BirdMockData.GetRightArgBirdCreateDTOS();

        if(list.Count == 0)
            throw new Exception("Can get data");

        foreach(var createDto in list)
        {
            createDto.OwnerId = AccountId;
            createDto.BirdTypeId = BirdTypeId;
        }

        _rightArgBirdCreateDTOList.AddRange(list);

        //Get bird update DTO list
        BirdRequest = MockData.BirdMockData.GetBirdUpdateDTOs();
        if(BirdRequest.Length == 0)
            throw new Exception("Can get data");

    }

    //TEST CASE 1: GET ALL BIRDS WHEN HAVE NO DATA
    [Test, Order(1)]
    public async Task GetBirds_ThrowsNotFoundException_WhenHaveNoData()
    {
        // Act, Assert
        Assert.ThrowsAsync<NotFoundException>(() => _birdServices.GetBirds());
    }

    //TEST CASE 2: GET BIRD BY BIRD ID WHEN HAVE NO DATA
    static Guid?[] NullAndEmptyGuid = new Guid?[] { null, Guid.Empty };

    [Test, Order(2)]
    [TestCaseSource("NullAndEmptyGuid")]
    public async Task GetBird_ThrowsNotFoundException_WhenHaveNoData(Guid birdId)
    {
        //Act, Assert
        Assert.CatchAsync<NotFoundException>(() => _birdServices.GetBird(Guid.Empty));
    }

    //TEST CASE 3: ADD BIRD WHEN GIVEN WRONG ARGUMENT (ownerId is wrong) 
    static List<BirdCreateDTO> _wrongArgBirdCreateDTOList = new List<BirdCreateDTO>();

    static List<BirdCreateDTO>[] TestResource3 = new List<BirdCreateDTO>[]
    {
        _wrongArgBirdCreateDTOList
    };

    [Test, Order(3)]
    [TestCaseSource("TestResource3")]
    public async Task AddBird_ThrowsBadRequestException_WhenGivenWrongOwnerId(List<BirdCreateDTO> birdCreateDataList)
    {
        //Act, Assert
        foreach(var data in birdCreateDataList)
        {
            Assert.CatchAsync<BadRequestException>(() => _birdServices.AddBird(data));

        }
    }
/// thieu 1 test case AddBird_ThrowsBadRequestException_WhenGivenWrongBirdTypeId

    //TEST CASE 4: ADD BIRD WHEN GIVEN RIGHT ARGUMENT
    static List<BirdCreateDTO> _rightArgBirdCreateDTOList = new List<BirdCreateDTO>();

    static List<BirdCreateDTO>[] TestResource4 = new List<BirdCreateDTO>[]
    {
        _rightArgBirdCreateDTOList
    };
 
    [Test, Order(4)]
    [TestCaseSource("TestResource4")]
    public async Task AddBird_ReturnGuid_WhenGivenRightData(List<BirdCreateDTO> birdCreateDataList)
    {
        Guid newBirdId = Guid.Empty;
        //Act and Assert
        foreach(var data in birdCreateDataList)
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                newBirdId = await _birdServices.AddBird(data);
            });
            var entity = await _wapperRepository.Bird.GetByIdActiveAsync(newBirdId);
            Assert.IsNotNull(entity);
            Assert.AreEqual(data.Name, entity.Name);
            Assert.AreEqual(data.Elo, entity.Elo);
            Assert.AreEqual(data.Age, entity.Age);
            BirdId = newBirdId;
        }
    }

    //TEST CASE 5: UPDATE BIRD WHEN GIVEN WRONG ARGUMENT (BirdId is wrong)
    [Test, Order(5)]
    public async Task UpdateBird_ThrowsBadRequestException_WhenGivenWrongBirdId()
    {
        //Act and Assert
        Assert.ThrowsAsync<BadRequestException>(() => _birdServices.UpdateBird(Guid.NewGuid(), BirdRequest[0]));
    }

    //TEST CASE 6: UPDATE BIRD WHEN GIVEN WRONG ARGUMENT (BirdTypeId is wrong)
    [Test, Order(6)]
    public async Task UpdateBird_ThrowsBadRequestException_WhenGivenWrongBirdTypeId()
    {
        //Act and Assert
        Assert.ThrowsAsync<BadRequestException>(() => _birdServices.UpdateBird(BirdId, BirdRequest[0]));
    }

    //TEST CASE 7: UPDATE BIRD WHEN GIVEN RIGHT ARGUMENT
    [Test, Order(7)]
    public async Task UpdateBird_ReturnTrue_WhenGivenRightData()
    {
        var birdUpdate = BirdRequest[0];
        birdUpdate.BirdTypeId = BirdTypeId;
        
        //Act and Assert
        Assert.DoesNotThrowAsync(async () =>
        {
            await _birdServices.UpdateBird(BirdId, birdUpdate);
        });
        var entity = await _wapperRepository.Bird.GetByIdActiveAsync(BirdId);
        Assert.IsNotNull(entity);
        Assert.AreEqual(BirdRequest[0].Name, entity.Name);
        Assert.AreEqual(BirdRequest[0].Elo, entity.Elo);
        Assert.AreEqual(BirdRequest[0].Age, entity.Age);
    }

    //TEST CASE 8: DELETE BIRD WHEN GIVEN WRONG ARGUMENT (BirdId is not exist)
    [Test, Order(8)]
    public async Task DeleteBird_ThrowsBadRequestException_WhenGivenWrongBirdId()
    {
        //Act and Assert
        Assert.ThrowsAsync<BadRequestException>(() => _birdServices.DeleteBird(Guid.NewGuid()));
    }

    //TEST CASE 9: DELETE BIRD WHEN GIVEN RIGHT ARGUMENT
    [Test, Order(9)]
    public async Task DeleteBird_ReturnTrue_WhenGivenRightData()
    {
        //Act and Assert
        Assert.DoesNotThrowAsync(async () =>
        {
            await _birdServices.DeleteBird(BirdId);
        });
        var entity = await _wapperRepository.Bird.GetByIdActiveAsync(BirdId);
        Assert.IsNull(entity);
    }


    public void Dispose()
    {
        // Delete in memory database
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}