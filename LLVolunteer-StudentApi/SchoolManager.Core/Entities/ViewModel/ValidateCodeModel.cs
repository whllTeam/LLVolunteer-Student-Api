using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SchoolManager.Core.Entities.ViewModel
{
    public class ValidateCodeModel
    {
        public string PreUrl { get; set; }
        public Stream ImageStream { get; set; }
        public string ImageBase64 { get; set; }
        public byte[] Arr { get; set; }
        public string LoginUrl { get; set; }
        public string LoginViewState { get; set; }
    }
}
