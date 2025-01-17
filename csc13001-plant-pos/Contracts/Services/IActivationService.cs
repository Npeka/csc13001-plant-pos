namespace csc13001_plant_pos.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
