using Cysharp.Threading.Tasks;

namespace Modules.SceneLoading
{
    public interface IOperation
    {
        bool IsDone { get; }

        UniTask Run();
    }
}