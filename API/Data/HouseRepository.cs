using Microsoft.EntityFrameworkCore;


public interface IHouseRepository
{
    Task<List<HouseDto>> GetAll();
    Task<HouseDetailDto?> Get(int id);
    Task<HouseDetailDto> Add(HouseDetailDto dto);
    Task<HouseDetailDto> Update(HouseDetailDto dto);
    Task Delete(int id);

}

public class HouseRepository : IHouseRepository
{
    private readonly HouseDbContext context;
    public HouseRepository(HouseDbContext context)
    {
        this.context = context;
    }

    public async Task<List<HouseDto>> GetAll()
    {
        return await context.Houses.Select(h => new HouseDto(h.Id, h.Address, h.Country, h.Price)).ToListAsync();
    }

    public async Task<HouseDetailDto?> Get(int id)
    {
        var entity = await context.Houses.SingleOrDefaultAsync(
            h => h.Id == id);
        if (entity == null)
            return null;
        else
            return EntityToDetailDto(entity);

    }

    public async Task<HouseDetailDto> Add(HouseDetailDto dto)
    {
        var entity = new HouseEntity();
        DtoToEntity(dto, entity);
        context.Houses.Add(entity);
        await context.SaveChangesAsync();
        return EntityToDetailDto(entity);

    }
    public async Task<HouseDetailDto> Update(HouseDetailDto dto)
    {
        var entity = await context.Houses.FindAsync(dto.Id);
        if (entity == null)
            throw new ArgumentException($"Error updating house {dto.Id}");
        DtoToEntity(dto, entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return EntityToDetailDto(entity);

    }

    public async Task Delete(int id)
    {
        var entity = await context.Houses.FindAsync(id);
        if (entity == null)
            throw new ArgumentException($"Error deleting house {id}");
        context.Houses.Remove(entity);
        await context.SaveChangesAsync();
    }
    private static void DtoToEntity(HouseDetailDto dto, HouseEntity entity)
    {
        entity.Address = dto.Address;
        entity.Country = dto.Country;
        entity.Description = dto.Description;
        entity.Price = dto.Price;
        entity.Photo = dto.Photo;
    }
    private static HouseDetailDto EntityToDetailDto(HouseEntity e)
    {
        return new HouseDetailDto(e.Id, e.Address, e.Country, e.Price,e.Description, e.Photo);
    }
}