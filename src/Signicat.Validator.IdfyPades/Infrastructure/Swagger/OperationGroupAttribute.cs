using System;

namespace Signicat.Validator.IdfyPades.Infrastructure.Swagger
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OperationGroupAttribute : Attribute
    {
        /// <summary>
        /// Group controller actions under a custom tag when generating Swagger docs
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public OperationGroupAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        /// <summary>
        /// Operation group name
        /// </summary>
        public string Name { get; set; }
    }
}