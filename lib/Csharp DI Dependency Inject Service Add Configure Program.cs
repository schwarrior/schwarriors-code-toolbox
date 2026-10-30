// create service

public interface IMessageService
{
	void SendMessage(string message);
}

public class EmailMessageService : IMessageService
{
	public void SendMessage(string message)
	{
		Console.WriteLine($"Email message sent: {message}");
	}
}

// register service in Program.cs

var services = new ServiceCollection();
services.AddSingleton<IMessageService, EmailMessageService>();
var serviceProvider = services.BuildServiceProvider();

// use service (by injecting it into a class)

public class Worker
{
	private readonly IMessageService _messageService;

	public Worker(IMessageService messageService)
	{
		_messageService = messageService;
	}

	public void DoWork()
	{
		_messageService.SendMessage("Hello, Dependency Injection!");
	}
}

// use use the service by manually resolving it  
var worker = serviceProvider.GetRequiredService<Worker>();
messageService.SendMessage("Hello, Dependency Injection!");