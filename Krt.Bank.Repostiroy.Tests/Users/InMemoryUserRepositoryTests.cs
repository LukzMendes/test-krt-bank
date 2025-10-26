using Krt.Bank.Domain.Common.Pagination;
using Krt.Bank.Domain.Users;
using Krt.Bank.Infrastructure.Data.Repositories.Users;
using System.Reflection;
using static Krt.Bank.Domain.Users.User;

public class InMemoryUserRepositoryTests
{
    private readonly InMemoryUserRepository _repository;

    public InMemoryUserRepositoryTests()
    {
        _repository = new InMemoryUserRepository();
    }

    private User CreateUser(Guid id, string name = "João da Silva", string cpf = "12345678900")
    {
        var user = User.Create(name, cpf);
        typeof(User)
            .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .First(p => p.Name == "Id" && p.PropertyType == typeof(UserId))
            .SetValue(user, UserId.Create(id));
        return user;
    }

    [Fact]
    public async Task AddAsync_ShouldStoreUser()
    {
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);

        var result = await _repository.AddAsync(user);

        Assert.Equal(userId, result.Id.Value);
        var stored = await _repository.GetAsync(UserId.Create(userId));
        Assert.NotNull(stored);
        Assert.Equal(user.Name, stored.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReplaceUser()
    {
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);
        await _repository.AddAsync(user);

        var updatedUser = CreateUser(userId, "Maria Silva", user.CPF);
        var result = await _repository.UpdateAsync(updatedUser);

        Assert.Equal("Maria Silva", result.Name);
        var stored = await _repository.GetAsync(UserId.Create(userId));
        Assert.Equal("Maria Silva", stored.Name);
    }

    [Fact]
    public async Task RemoveAsync_ShouldDeleteUser()
    {
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);
        await _repository.AddAsync(user);

        await _repository.RemoveAsync(user);
        var result = await _repository.GetAsync(UserId.Create(userId));

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var user1 = CreateUser(Guid.NewGuid(), "User One", "11111111111");
        var user2 = CreateUser(Guid.NewGuid(), "User Two", "22222222222");

        await _repository.AddAsync(user1);
        await _repository.AddAsync(user2);

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Name == "User One");
        Assert.Contains(result, u => u.Name == "User Two");
    }

    [Fact]
    public async Task GetPaginated_ShouldReturnFilteredUsers()
    {
        var user1 = CreateUser(Guid.NewGuid(), "Alice", "11111111111");
        var user2 = CreateUser(Guid.NewGuid(), "Bob", "22222222222");
        var user3 = CreateUser(Guid.NewGuid(), "Alice", "33333333333");

        await _repository.AddAsync(user1);
        await _repository.AddAsync(user2);
        await _repository.AddAsync(user3);

        var paginate = Paginate.Create(

            page: 1,
            pageSize: 10,
            ascending: true,
            orderBy: null
        );

        var result = await _repository.GetPaginated(null, null, "Alice", null, paginate);

        Assert.Equal(2, result.Items.Count);
        Assert.All(result.Items, u => Assert.Equal("Alice", u.Name));
    }
}