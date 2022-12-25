﻿using CleanArchMvc.Domain.Validation;

namespace CleanArchMvc.Domain.Entities
{
    public sealed class Category : Entity
    {
        public string Name { get; private set; }

        public Category(string name)
        {
            ValidationDomain(name);
        }

        public Category(int id, string name)
        {
            DomainExceptionValidation.When(id < 0, "Invalid Id value");
            Id = id;
            ValidationDomain(name);
        }

        public void Update(string name)
        {
            ValidationDomain(name);
        }

        public ICollection<Product> Products { get; set; }   
        
        private void ValidationDomain(string name)
        {
            DomainExceptionValidation.When(
                string.IsNullOrEmpty(name), 
                "Invalid name, name is required");

            DomainExceptionValidation.When(
                name.Length < 3,
                "Invalid name, too short, minimun 3 charecters");

            Name = name;
        }

    }
}
