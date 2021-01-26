namespace AiurTodo.CSharp.Models.TokenModels
{
    public class CodeToOpenIdViewModel
    {
        public int Code { get; set; }
        public string OpenId { get; set; }
        public string Scope { get; set; }
        public string Message { get; set; }
    }
}