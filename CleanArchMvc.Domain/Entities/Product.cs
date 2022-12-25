﻿using CleanArchMvc.Domain.Validation;

namespace CleanArchMvc.Domain.Entities
{
    public sealed class Product : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public string Image { get; private set; }

        public Product(int id, string name, string description, decimal price, int stock, string image)
        {
            DomainExceptionValidation.When(id < 0, "Invalid Id value");
            Id = id;
            ValidationDomain(name, description, price, stock, image);
        }

        public Product(string name, string description, decimal price, int stock, string image)
        {
            ValidationDomain(name, description, price, stock, image);
        }

        public void Update (string name, string description, decimal price, int stock, string image, int categoryId)
        {
            ValidationDomain(name, description, price, stock, image);
            CategoryId = categoryId;
        }

        private void ValidationDomain(string name, string description, decimal price, int stock, string image)
        {
            DomainExceptionValidation.When(
                string.IsNullOrEmpty(name),
                "Invalid name, name is required");

            DomainExceptionValidation.When(
                name.Length < 3,
                "Invalid name, too short, minimun 3 charecters");

            DomainExceptionValidation.When(
                string.IsNullOrEmpty(description),
                "Invalid description, name is required");

            DomainExceptionValidation.When(
                description.Length < 5,
                "Invalid description, too short, minimun 5 charecters");

            DomainExceptionValidation.When(
                price < 0,
                "Invalid price value");

            DomainExceptionValidation.When(
                stock < 0,
                "Invalid stock value");

            DomainExceptionValidation.When(
                image?.Length > 250,
                "Invalid image name, too long, maximum 250 characters");

            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            Image = image;
        }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
