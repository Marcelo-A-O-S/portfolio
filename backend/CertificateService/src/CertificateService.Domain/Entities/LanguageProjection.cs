namespace CertificateService.Domain.Entities
{
    public class LanguageProjection
    {
        public Guid Id { get; private set; }
        public Guid LanguageId { get; private set; }
        public string Code {get; private set;}
        public string Name { get; private set;}
        public LanguageProjection(
            Guid languageId,
            string code,
            string name
        )
        {
            this.LanguageId = languageId;
            this.Code = code;
            this.Name = name;
        }
        public void GenerateId()
        {
            this.Id = Guid.NewGuid();
        }
    }
}