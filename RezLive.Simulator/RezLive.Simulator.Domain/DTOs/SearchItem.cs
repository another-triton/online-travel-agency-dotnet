namespace RezLive.Simulator.Domain.DTOs;
public class SearchItem  {    
    public SearchItem() {}
    public SearchItem(int supplierId, string xml) {
        SupplierId = supplierId;
        Xml = xml;
    }
    public readonly int SupplierId;
    public readonly string Xml = null!;
}