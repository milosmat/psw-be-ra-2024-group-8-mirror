using Explorer.API.Controllers.Tourist.Rating;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Integration;


[Collection("Sequential")]
public class Tests : BasePaymentsIntegrationTest
{
    public Tests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_all()
    {

    }



}
