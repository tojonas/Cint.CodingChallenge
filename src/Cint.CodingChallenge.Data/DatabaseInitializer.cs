using Cint.CodingChallenge.Model;

namespace Cint.CodingChallenge.Data
{
    //https://jasontaylor.dev/ef-core-database-initialisation-strategies/
    public class DatabaseInitializer
    {
        private readonly ApplicationDbContext _ctx;

        public DatabaseInitializer(ApplicationDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task InitializeAsync()
        {
            await _ctx.Database.EnsureDeletedAsync();
            await _ctx.Database.EnsureCreatedAsync();
            await AddDataAsync();
        }

        private async Task AddDataAsync()
        {
            var surveys = new Survey[] {
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Fruit Survey",
                    Description = "Survey about Fruit",
                    LengthMinutes = 7,
                    IncentiveEuros = 2.34
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Another Fruit Survey",
                    Description = "Another Survey about Fruit",
                    LengthMinutes = 5,
                    IncentiveEuros = 2.34
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "A Third Fruit Survey",
                    Description = "Another Survey about Fruit",
                    LengthMinutes = 6,
                    IncentiveEuros = 2.34
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Pet Survey",
                    Description = "Survey about Pets",
                    LengthMinutes = 9,
                    IncentiveEuros = 3.78
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Hobby Survey",
                    Description = "Survey about Hobbies",
                    LengthMinutes = 4,
                    IncentiveEuros = 4.51
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Travel Survey",
                    Description = "Survey about Travel Destinations",
                    LengthMinutes = 15,
                    IncentiveEuros = 2.09
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Food Survey",
                    Description = "Survey about Favorite Foods",
                    LengthMinutes = 2,
                    IncentiveEuros = 1.12
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Car Survey",
                    Description = "Survey about Cars",
                    LengthMinutes = 7*2,
                    IncentiveEuros = 2.34*2
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Video Games Survey",
                    Description = "Survey about Video Games",
                    LengthMinutes = 9 * 2,
                    IncentiveEuros = 3.78 * 2
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Gardening Survey",
                    Description = "Survey about Gardening",
                    LengthMinutes = 4 * 2,
                    IncentiveEuros = 4.51 * 2
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Cat Survey",
                    Description = "Survey about Cats",
                    LengthMinutes = 15 * 2,
                    IncentiveEuros = 2.09*2
                },
                new Survey {
                    Id = Guid.NewGuid(),
                    Name = "Music Survey",
                    Description = "Survey about Music",
                    LengthMinutes = 2*2,
                    IncentiveEuros = 1.12 * 2
                }
            };
            await _ctx.Surveys.AddRangeAsync(surveys);
            await _ctx.SaveChangesAsync();
        }
    }
}
