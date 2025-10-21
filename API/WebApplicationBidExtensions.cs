using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationBidExtensions
{
    public static void mapBidEndpoints(this WebApplication app)
    {
        app.MapGet("house/{houseId:int}/bids", async (int houseId,
            IHouseRepository houseRepo, IBidRepository bidRepo) =>
            {
                if (await houseRepo.Get(houseId) == null)
                    return Results.Problem($"House {houseId} not found", statusCode: 404);

                var bids = await bidRepo.Get(houseId);
                return Results.Ok(bids);
            }).ProducesProblem(404).Produces(StatusCodes.Status200OK);

        app.MapPost("house/{houseId:int}/bids", async (int houseId, [FromBody]BidDto bidDto, IBidRepository bidRepo) =>
            {
                if (bidDto.HouseId != houseId)
                    return Results.Problem("No match.", statusCode: StatusCodes.Status400BadRequest);
                if (!MiniValidator.TryValidate(bidDto, out var errors))
                    return Results.ValidationProblem(errors);
                var newBid = await bidRepo.Add(bidDto);
                return Results.Created($"house/{newBid.HouseId}/bids", newBid);
            }).ProducesValidationProblem().ProducesProblem(404).Produces<BidDto>(StatusCodes.Status201Created);
    }
}