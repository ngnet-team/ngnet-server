namespace Ngnet.Services.Email
{
    public class EmailSendetModel
    {
        public string FromAddress { get; set; }

        public string FromName { get; set; }

        public string ToAddress { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
