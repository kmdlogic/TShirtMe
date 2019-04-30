using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kmd.Logic.Audit.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TShirtMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private static readonly Random Random = new Random();
        private readonly IAudit _audit;
        private readonly TShirtMeContext _ctx;

        public EntryController(TShirtMeContext ctx, IAudit audit)
        {
            _ctx = ctx;
            _audit = audit;
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubmitEntryRequest request)
        {
            if (request.Code.ToLower() != "steam2019")
            {
                return UnprocessableEntity(
                    new
                    {
                        Message = "The secret code is invalid"
                    });
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //generate entry and send sms.
                var entry = await SubmitEntry(request.PhoneNumber);

                _audit.Write("The entry {@Entry} has been accepted", entry);

                return Ok(entry);
            }
            catch (Exception ex)
            {
                //log message 
                return UnprocessableEntity(
                    new
                    {
                        ex.Message,
                        Exception = ex
                    });
            }
        }

        [HttpGet]
        [Route("{entryCode}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] string entryCode)
        {
            try
            {
                if (User.Claims.Any(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && x.Value == "TShirtMeAdminRole"))
                {
                    return Ok(EntryExists(entryCode));
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                //log message 
                return UnprocessableEntity(
                    new
                    {
                        ex.Message,
                        Exception = ex
                    });
            }
        }

        private bool EntryExists(string entryCode)
        {
            var entry = _ctx.Entries.SingleOrDefault(x => x.EntryCode == entryCode);
            if (entry == null)
            {
                return false;
            }

            if (entry.Claimed)
            {
                throw new Exception("This code has already been claimed");
            }

            entry.Claimed = true;
            _ctx.SaveChanges();
            return true;
        }

        private bool PhoneExists(string phoneNumber)
        {
            return _ctx.Entries.Any(x => x.PhoneNumber == phoneNumber);
        }


        private async Task<object> SubmitEntry(string phoneNumber)
        {
            phoneNumber = phoneNumber.ToLower();
            if (PhoneExists(phoneNumber))
            {
                throw new Exception("This phone number has already been entered");
            }

            //naive collision detection
            string entryCode;
            do
            {
                entryCode = RandomString(5);
            } while (EntryExists(entryCode));

            var newEntry = new EntryEntity
            {
                PhoneNumber = phoneNumber.ToLower(),
                EntryCode = entryCode
            };

            _ctx.Entries.Add(newEntry);


            await SMSClient.SendSms(phoneNumber, newEntry.EntryCode);

            await _ctx.SaveChangesAsync();


            return new
            {
                newEntry.EntryCode,
                PhoneNumber = phoneNumber
            };
        }

        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz1234567890";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }

    public class SubmitEntryRequest
    {
        [Required] public string PhoneNumber { get; set; }

        [Required] public string Code { get; set; }
    }
}