using System.Threading.Tasks;

namespace csc13001_plant_pos_frontend.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
