using AutoMapper;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Auth;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using EBird.Infrastructure.Repositories;
using EBird.Test.MockData;
using Microsoft.EntityFrameworkCore;

namespace EBird.Test.System.Services
{
    //In memory database
    public class AccountServicesTest : IDisposable
    {
        protected readonly ApplicationDbContext _context;
        private readonly IAccountServices _accountServices;
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IMapper _mapper; //AutoMapper
        
        public AccountServicesTest()
        {
            //In memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "EBird")
                .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            //Repository
            _accountRepository = new GenericRepository<AccountEntity>(_context);
            //AutoMapper
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountEntity, AccountResponse>();
            });
            _mapper = configuration.CreateMapper();
            //Service
            _accountServices = new AccountServices(_accountRepository, _mapper);
        }

        //Test case 1: Get all account when data not found
        [Test, Order(1)]
        public async Task GetAllAccount_ReturnEmptyList_WhenDataNotFound(){
            
            // Actual values
            var result = await _accountServices.GetAllAccount();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        //Test case 2: Get all account when data found
        [Test, Order(2)]
        public async Task GetAllAccount_ReturnData_WhenDataFound()
        {
            // Expected values are in ACCOUNTS_EXPECTED.json
            for (int i = 0; i < AccountMockData.GetAccountList().Count; i++)
            {
                await _accountRepository.CreateAsync(AccountMockData.GetAccountList()[i]);
            }

            // Actual values
            var result = await _accountServices.GetAllAccount();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(AccountMockData.GetAccountList().Count, result.Count);

        }

        
        
        public void Dispose()
        {
            //Delete all data in database
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

