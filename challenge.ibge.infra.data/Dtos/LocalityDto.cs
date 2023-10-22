namespace challenge.ibge.infra.data.Dtos;

public class LocalityDto
{
    public int Id { get; set; }
    
    /// <summary>
    /// To be valid, should contains a sequence with exactly 7 numbers
    /// </summary>
    public int IbgeCode { get; set; }
    
    /// <summary>
    /// To be valid, should contains minimum 3 letters
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// To be valid, should contains 2 letters (PR/SC/SP)
    /// </summary>
    public string UF { get; set; }
}