using System;
using Bogus;
using NodeServicesBenchmark.Website.Models;
using NodeServicesBenchmark.Website.Models.Loops;

namespace NodeServicesBenchmark.Website.Extensions
{
    public static class LoopModelExtensions
    {
        public static LoopModel GenerateLoopModel(this LoopModel loopModel)
        {
            if (loopModel == null) throw new ArgumentNullException(nameof(loopModel));

            var loopItem = new Faker<LoopItem>()
                .RuleFor(l => l.Name, f => f.Company.CompanyName())
                .RuleFor(l => l.Address, f => f.Address.FullAddress())
                .RuleFor(l => l.Description, f => f.Company.CatchPhrase())
                .RuleFor(l => l.ImageUrl, _ => "https://loremflickr.com/g/320/240/brutalist")
                .RuleFor(l => l.Price, f => f.Finance.Amount());

            loopModel = new Faker<LoopModel>()
                .RuleFor(l => l.LoopItems, f => loopItem.Generate(250))
                .Generate();

            return loopModel;
        }
    }
}
