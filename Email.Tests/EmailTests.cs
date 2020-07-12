using Email.Services.DTOs;
using Email.Services.Emails;
using Email.Services.Emails.Commands;
using Email.Services.Emails.Queries;
using Email.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Email.Tests
{
    public class EmailTests : IEmailRepository, IEmailServiceResponder, IEmailClient
    {
        [Fact]
        public async void CreateEmail()
        {
            var handler = new CreateEmailCommandHandler(this,this);

            await handler.Handle(new CreateEmailCommand("Subject", "Author", "Body"), new System.Threading.CancellationToken());

            Assert.Equal("Subject", testSubjet);
            Assert.Equal("Author", testauthor);
            Assert.Equal("Body", testBody);
        }


        [Fact]
        public async void AddRecipient_ToNotExistingEmail_NotExistsResult()
        {
            var handler = new AddRecipientCommandHandler(this,this);

            await handler.Handle(new AddRecipientCommand("100",  new List<EmailAddressDto> { new EmailAddressDto() { Address = "toemail01@mail.com", Name = "A" } }), new System.Threading.CancellationToken());

            Assert.Equal(EmailResult.NotExists,testResult);
        }

        [Fact]
        public async void AddRecipient_EmailThatAleardyExists_AlreadyExistsResult()
        {
            var handler = new AddRecipientCommandHandler(this, this);
            CreateNew("Subject", "Author", "Body");
            testToEmails = new List<string> { "toemail01@mail.com" , "toemail02@mail.com" };

            await handler.Handle(new AddRecipientCommand("1", new List<EmailAddressDto> { new EmailAddressDto() { Address = "toemail01@mail.com", Name = "A" } }), new System.Threading.CancellationToken());

            Assert.Equal(EmailResult.AlreadyExists, testResult);
        }

        [Fact]
        public async void AddRecipient_NewFromEmail_SuccessResult()
        {
            var handler = new AddRecipientCommandHandler(this, this);
            CreateNew("Subject", "Author", "Body");
            testToEmails.Add( "toemail02@mail.com" );

            await handler.Handle(new AddRecipientCommand("1", new List<EmailAddressDto> { new EmailAddressDto() { Address = "toemail01@mail.com", Name = "A" } }), new System.Threading.CancellationToken());

            Assert.Equal(EmailResult.Success, testResult);
            Assert.Equal(2, testToEmails.Count);
            Assert.Contains(testToEmails, x => x == "toemail01@mail.com");
        }

        [Fact]
        public async void AddSender_NewToEmail_SuccessResult()
        {
            var handler = new AddSenderCommandHandler(this, this);
            CreateNew("Subject", "Author", "Body");

            await handler.Handle(new AddSenderCommand("1", new EmailAddressDto() { Address = "fromemail01@mail.com", Name = "A" }), new System.Threading.CancellationToken());

            Assert.Equal(EmailResult.Success, testResult);
            Assert.Equal("fromemail01@mail.com", testFromEmail);
        }

        [Fact]
        public async void GetEmailStatus_ToNotExistingEmail_NotExistsResult()
        {
            var handler = new GetEmailStatusQueryHandler(this);

            var result = await handler.Handle(new GetEmailStatusQuery("100"), new System.Threading.CancellationToken());

            Assert.Equal(EmailResult.NotExists, result.ErrorCode);
        }

        [Fact]
        public async void GetEmailStatus_ExistingPendingEmail_ReturmPending()
        {
            var handler = new GetEmailStatusQueryHandler(this);
            CreateNew("Subject", "Author", "Body");

            var result = await handler.Handle(new GetEmailStatusQuery("1"), new System.Threading.CancellationToken());

            Assert.Null(result.ErrorCode);
            Assert.Equal("pending", result.Data);

        }

        [Fact]
        public async void GetEmailDetails_ToNotExistingEmail_NotExistsResult()
        {
            var handler = new GetEmailDetailsQueryHandler(this);

            var result = await handler.Handle(new GetEmailDetailsQuery("100"), new System.Threading.CancellationToken());

            Assert.Equal(EmailResult.NotExists, result.ErrorCode);
        }

        [Fact]
        public async void GetEmailDetails_ExistingPendingEmail_ReturmData()
        {
            var handler = new GetEmailDetailsQueryHandler(this);
            CreateNew("Subject", "Author", "Body");

            var result = await handler.Handle(new GetEmailDetailsQuery("1"), new System.Threading.CancellationToken());

            Assert.Null(result.ErrorCode);
            Assert.Equal("Subject", result.Data.Subject);
            Assert.Equal("Author", result.Data.Author);

        }

        [Fact]
        public async void GetAll_ReturnTwoExisting()
        {
            var handler = new GetAllEmailsQueryHandler(this);

            var result = await handler.Handle(new GetAllEmailsQuery(), new System.Threading.CancellationToken());

            Assert.Null(result.ErrorCode);
            Assert.Equal(2, result.Data.Count());

        }

        [Fact]
        public async void SendAllPending_getTwoDocuments_sendTwoEmails()
        {
            var handler = new SendAllPendingCommandHandler(this,this,this);

            await handler.Handle(new SendAllPendingCommand(), new System.Threading.CancellationToken());

            Assert.Equal(EmailResult.Success, testResult);
            Assert.Equal(2, testEmailSentCount);

        }

        #region mocking

        string testSubjet;
        string testauthor;
        List<string> testToEmails = new List<string>();           
        string testBody;
        string testEmailID;
        EmailResult testResult;
        string testFromEmail;
        int testEmailSentCount;



        public string CreateNew(string subjet, string author,  string body)
        {
            testSubjet = subjet;
            testauthor = author;
            testBody =  body;
            //testToEmails = toemails.ToList();
            testEmailID = "1";
            return testEmailID;
        }

        public void SetResult(EmailResult result)
        {
            testResult = result;
        }

        public EmailEntity GetById(string emailId)
        {
            if (testEmailID != emailId)
                return null;
           
            var e =  new EmailEntity(testEmailID,testauthor,testSubjet,testBody);

            if (testToEmails.Any())
                e.AddRecipients(testToEmails.Select(x => new EmailAddress(x,"A")).ToList());

            return e;

        }

        public IEnumerable<EmailEntity> GetAllEmails()
        {
            return new List<EmailEntity>() {
                new EmailEntity("1","Subject1", "Author1", "Body1"),
                new EmailEntity("2","Subject2", "Author2", "Body2")
            };
        }

        public IEnumerable<EmailEntity> GetPendingEmails()
        {
            return new List<EmailEntity>() {
                new EmailEntity("1","Subject1", "Author1", "Body1"),
                new EmailEntity("2","Subject2", "Author2", "Body2")
            };
        }



        public async Task Send(EmailEntity email)
        {
            testEmailSentCount++;
        }

        public Task Update(EmailEntity entity)
        {
            testToEmails = entity.GetRecipients().Select(x => x.Address).ToList();
            testFromEmail = entity.GetSender()?.Address;
            return Task.CompletedTask;
        }

        public void SetCreatedId(string id)
        {
            
        }

        #endregion
    }
}
