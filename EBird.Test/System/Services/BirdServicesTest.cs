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

        //create testing account 
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

        //create birdTypeId
        BirdTypeEntity birdType = new BirdTypeEntity()
        {
            TypeName = "Test",
            TypeCode = "TEST",
            CreatedDatetime = DateTime.Now
        };
        _wapperRepository.BirdType.CreateAsync(birdType);
        BirdTypeId = birdType.Id;

        //get list wrong arg
        var list = MockData.BirdMockData.GetWrongArgBirdCreateDTOs();

        if(list.Count == 0)
            throw new Exception("Can get data");
        _wrongArgBirdCreateDtoList.AddRange(list);

        //get list right arg
        list = MockData.BirdMockData.GetRightArgBirdCreateDTOS();

        if(list.Count == 0)
            throw new Exception("Can get data");

        foreach(var createDto in list)
        {
            createDto.OwnerId = AccountId;
            createDto.BirdTypeId = BirdTypeId;
        }

        _rightArgBirdCreateDtoList.AddRange(list);

        //get list bird update dto 
        BirdRequest = MockData.BirdMockData.GetBirdUpdateDTOs();
        if(BirdRequest.Length == 0)
            throw new Exception("Can get data");

    }

    //Test case 1: Get all bird when dont have data
    [Test, Order(1)]
    public async Task GetAllBird_ThrowExceptionNotFoundException_WhenHaveNotData()
    {
        // Act, Assert
        Assert.ThrowsAsync<NotFoundException>(() => _birdServices.GetBirds());
    }

    //Test case 2: Get bird by it's id when dont have data 
    static Guid?[] NullAndEmptyGuid = new Guid?[] { null, Guid.Empty };

    [Test, Order(2)]
    [TestCaseSource("NullAndEmptyGuid")]
    public async Task GetBirdWithId_ThrowExceptionNotFoundException_WhenHavenNotData(Guid birdId)
    {
        //Act and Assert
        Assert.CatchAsync<NotFoundException>(() => _birdServices.GetBird(Guid.Empty));
    }

    //Test case 3: Create bird with wrong argument (wrong ownerId, birdTypeId) 
    static List<BirdCreateDTO> _wrongArgBirdCreateDtoList = new List<BirdCreateDTO>();

    static List<BirdCreateDTO>[] TestResource3 = new List<BirdCreateDTO>[]
    {
        _wrongArgBirdCreateDtoList
    };

    [Test, Order(3)]
    [TestCaseSource("TestResource3")]
    public async Task CreateBird_ThrowBadRequestException_WhenGivenWrongOwnerId(List<BirdCreateDTO> birdCreateDataList)
    {
        //Act and Assert
        foreach(var data in birdCreateDataList)
        {
            Assert.CatchAsync<BadRequestException>(() => _birdServices.AddBird(data));

        }
    }

    //Test case : create bird with right argument 
    static List<BirdCreateDTO> _rightArgBirdCreateDtoList = new List<BirdCreateDTO>();

    static List<BirdCreateDTO>[] TestResource4 = new List<BirdCreateDTO>[]
    {
        _rightArgBirdCreateDtoList
    };

    [Test, Order(4)]
    [TestCaseSource("TestResource4")]
    public async Task CreateBird_ReturnGuid_WhenGivenRightData(List<BirdCreateDTO> birdCreateDataList)
    {
        Guid createdId = Guid.Empty;
        //Act and Assert
        foreach(var data in birdCreateDataList)
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                createdId = await _birdServices.AddBird(data);
            });
            var entity = await _wapperRepository.Bird.GetByIdActiveAsync(createdId);
            Assert.IsNotNull(entity);
            Assert.AreEqual(data.Name, entity.Name);
            Assert.AreEqual(data.Elo, entity.Elo);
            Assert.AreEqual(data.Age, entity.Age);
            BirdId = createdId;
        }
    }

    //Test case : create bird with wrong argument (wrong bird Id, wrongBirdTypeId)
   
    
    [Test, Order(5)]
    public async Task UpdateBird_ThrowException_WhenGivenWrongBirdId()
    {
        //Act and Assert
        Assert.ThrowsAsync<BadRequestException>(() => _birdServices.UpdateBird(Guid.NewGuid(), BirdRequest[0]));
    }


    [Test, Order(6)]
    public async Task UpdateBird_ThrowException_WhenGivenWrongBirdTypeId()
    {
        //Act and Assert
        Assert.ThrowsAsync<BadRequestException>(() => _birdServices.UpdateBird(BirdId, BirdRequest[0]));
    }

    //Test case : update bird with right argument
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

    //Test case : delete bird with wrong argument (wrong bird Id)
    [Test, Order(8)]
    public async Task DeleteBird_ThrowException_WhenGivenWrongBirdId()
    {
        //Act and Assert
        Assert.ThrowsAsync<BadRequestException>(() => _birdServices.DeleteBird(Guid.NewGuid()));
    }

    //Test case : delete bird with right argument
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