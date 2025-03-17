using System.Threading.Tasks;

namespace csc13001_plant_pos_frontend.Activation;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
