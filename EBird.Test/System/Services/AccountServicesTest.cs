using AutoMapper;
using EBird.Application.Exceptions;
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
        //Test case 3: Get account by id when data not found
        [Test, Order(3)]
        public async Task GetAccountById_ThrowNotFoundException_WhenDataNotFound()
        {
            var accountId = Guid.NewGuid();
            
            // Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _accountServices.GetAccountById(accountId));
        }
        //Test case 4: Get account by id when data found
        [Test, Order(4)]
        public async Task GetAccountById_ReturnData_WhenDataFound()
        {
            var account = AccountMockData.GetAccount();
            await _accountRepository.CreateAsync(account);
            

            // Actual values
            var result = await _accountServices.GetAccountById(account.Id);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(account.Id, result.Id);
            Assert.AreEqual(account.Email, result.Email);
            Assert.AreEqual(account.FirstName, result.FirstName);
            Assert.AreEqual(account.LastName, result.LastName);
        }
        


        
        
        public void Dispose()
        {
            //Delete all data in database
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

