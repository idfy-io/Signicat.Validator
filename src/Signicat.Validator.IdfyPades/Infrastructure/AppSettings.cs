namespace Signicat.Validator.IdfyPades.Infrastructure
{
    public class AppSettings
    {
        public SeqSettings Seq { get; set; }
    }

    public class SeqSettings
    {
        public string Url { get; set; }

        public string ApiKey { get; set; }
    }
}