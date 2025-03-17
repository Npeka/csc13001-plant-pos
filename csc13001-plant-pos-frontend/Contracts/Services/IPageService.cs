using System;

namespace csc13001_plant_pos_frontend.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);
}
