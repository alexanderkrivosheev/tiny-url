namespace TinyURL.Commands
{
    public interface ICommand
    {
        string Name { get; }

        string Description { get; }

        Task<bool> RunAsyn(Dictionary<string, string> args);
    }
}
