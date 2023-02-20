using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Bird;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using EBird.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EBird.Test.System.Services;

public class BirdServicesTest : IDisposable
{
    protected readonly ApplicationDbContext _context;
    private readonly IBirdService _birdServices;
    private IWapperRepository _wapperRepository;
    private IMapper _mapper;

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
        //AutoMapper
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BirdEntity, BirdCreateDTO>();
        });
        _mapper = configuration.CreateMapper();
        //Service
        _birdServices = new BirdService(_wapperRepository, _mapper, null, null);
    }

    //Test case 1: Get all bird when data not found
    [Test, Order(1)]
    public async Task GetBirdList_ThrowNotFoundException_WhenDataNotFound()
    {
        // Arrange
        Assert.ThrowsAsync<NotFoundException>( () => _birdServices.GetBirds());
    }



    public void Dispose()
    {
        // Delete in memory database
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}