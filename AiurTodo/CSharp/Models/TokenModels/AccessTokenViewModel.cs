using System;

namespace AiurTodo.CSharp.Models.TokenModels
{
    public class AccessTokenViewModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public DateTime DeadTime { get; set; }
    }
}