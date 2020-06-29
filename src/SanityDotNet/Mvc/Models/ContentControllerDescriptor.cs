using System;

namespace SanityDotNet.Mvc.Models
{
    public class ContentControllerDescriptor
    {
        public Type ControllerType { get; set; }
        public Type ForContentType { get; set; }
    }
}