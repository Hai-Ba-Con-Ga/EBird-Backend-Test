using System.Net;
using System.Security.Claims;
using AutoMapper;
using EBird.Application.Hubs;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Response;

namespace EBird.Api.Controllers
{
    
    [Route("chat-room")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        
        private readonly IGenericRepository<ChatRoomEntity> _chatRoomRepository;
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IGenericRepository<ParticipantEntity> _participantRepository;
        private readonly IMapper _mapper;


        public ChatRoomController(IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<AccountEntity> accountRepository, IGenericRepository<ParticipantEntity> participantRepository, IMapper mapper)
        {
            _chatRoomRepository = chatRoomRepository;
            _accountRepository = accountRepository;
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<Response<List<ChatRoomEntity>>>> GetChatRoom()
        {

            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new Response<List<ChatRoomEntity>>();
            if (rawId == null)
            {
                response = Response<List<ChatRoomEntity>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage("Account not found");
                return StatusCode((int)response.StatusCode, response);
            }
            try
            {
                Guid accId = Guid.Parse(rawId);
                var chatRooms = (await _chatRoomRepository.WhereAsync(x => x.Participants.Any(y => y.AccountId == accId), "Participants.Account")).ToList();
                response = Response<List<ChatRoomEntity>>.Builder().SetData(chatRooms).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                response = Response<List<ChatRoomEntity>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ChatRoomEntity>>> GetChatRoomById(Guid id)
        {
            var chatRoom = (await _chatRoomRepository.WhereAsync(x => x.Id == id, "Participants.Account")).FirstOrDefault();
            var response = new Response<ChatRoomEntity>();
            response = Response<ChatRoomEntity>.Builder().SetData(chatRoom).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpPost("create-group")]
        public async Task<ActionResult<Response<string>>> CreateChatRoomGroup(CreateChatRoomGroup room)
        {
            var newChatRoom = new ChatRoomEntity()
            {
                Name = room.Name,
                TypeChatRoom = room.TypeChatRoom
            };

            await _chatRoomRepository.CreateAsync(newChatRoom);
           
            var response = Response<string>.Builder().SetMessage("Created Successfully").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpPost("create-private")]
        public async Task<ActionResult<Response<string>>> CreateChatRoomPrivate(CreateChatRoomPrivate room)
        {
            var account = await _accountRepository.GetByIdActiveAsync(room.ReceiverId);
            var response = new Response<string>();
            if (account == null)
            {
                response = Response<string>.Builder().SetMessage("Account not found").SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound);
                return StatusCode((int)HttpStatusCode.NotFound, response);
            }
            var newChatRoom = new ChatRoomEntity()
            {
                Name = account.FirstName + " " + account.LastName,
                TypeChatRoom = room.TypeChatRoom,
                Participants = new List<ParticipantEntity>()
                {
                    new ParticipantEntity()
                    {
                        AccountId = room.ReceiverId,
                        ChatRoomId = room.ReceiverId
                    },
                    new ParticipantEntity()
                    {
                        AccountId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        ChatRoomId =  room.ReceiverId
                    }
                }
            };

            await _chatRoomRepository.CreateAsync(newChatRoom);
    
            response = Response<string>.Builder().SetMessage("Created Successfully").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpPost("add-participant")]
        public async Task<ActionResult<Response<string>>> AddParticipant(AddParticipant participant)
        {
            var chatRoom = await _chatRoomRepository.GetByIdAsync(participant.ChatRoomId);
            var response = new Response<string>();
            if (chatRoom == null)
            {
                response = Response<string>.Builder().SetMessage("Chat room not found").SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound);
                return StatusCode((int)HttpStatusCode.NotFound, response);
            }
            foreach (var chatRoomMember in participant.AccountId){
                var newParticipant = new ParticipantEntity()
                {
                    AccountId = chatRoomMember,
                    ChatRoomId = participant.ChatRoomId
                };
                await _participantRepository.CreateAsync(newParticipant);
            }
            response = Response<string>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteChatRoom(Guid id)
        {
            var deleteChatRoom = await _chatRoomRepository.DeleteSoftAsync(id);
            var response = Response<string>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }


    }

}