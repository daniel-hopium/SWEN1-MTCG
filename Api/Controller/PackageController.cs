﻿using API.HttpServer;
using Api.Utils;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Transversal.Entities;

namespace API.Controller;
public class PackageController
{
    private readonly PackageService _packageService;
    
    public PackageController(PackageService packageService)
    {
        _packageService = packageService;
    }
    
    public void ProcessRequest(object sender, HttpSvrEventArgs e)
    {

        if (e.Path.Equals("/packages") && e.Method.Equals("POST"))
        {
            CreatePackages(e);
        }
        else if (e.Path.Equals("/transactions/packages") && e.Method.Equals("POST"))
        {
            BuyPackage(e);
        }
    }

    private void CreatePackages(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }
        
        if (!Authorization.AuthorizeAdmin(e.Authorization))
        {
            e.Reply(403, "Provided user is not an admin");
            return;
        }
        
        var cards = JsonConvert.DeserializeObject<List<CardDto>>(e.Payload);
        try
        {
            _packageService.CreatePackage(cards!);
            e.Reply(201, "Package and cards successfully created");
        }
        catch (InvalidOperationException exception)
        {
            e.Reply(409, "At least one card in the packages already exists");
        }
        catch (Exception exception)
        {
            e.Reply(500, "Error creating package");
        }
    }
    
    private void BuyPackage(HttpSvrEventArgs e)
    {
        if (!Authorization.AuthorizeUser(e.Authorization))
        {
            e.Reply(401, "Unauthorized");
            return;
        }

        var package = JsonConvert.DeserializeObject<PackageDto>(e.Payload);
        var username = Authorization.GetUsernameFromAuthorization(e.Authorization);
        try
        {
            _packageService.BuyPackage(package, username);
            e.Reply(200, "Package successfully bought");
        }
        catch (InvalidOperationException exception)
        {
            e.Reply(404, exception.Message);
        }
        catch (InsufficientFundsException exception)
        {
            e.Reply(403, exception.Message);
        }
        catch (Exception exception)
        {
            e.Reply(500, "Error buying package");
        }
    }
}   


