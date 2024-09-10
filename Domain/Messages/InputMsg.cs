namespace WebApi.Domain.Messages;

public struct InputMsg
{
    public const string Required = "Este campo é obrigatório.";
    public const string Invalid = "Este dado é inválido.";
    public const string LengthMin2Max20 = "Este campo deve ter entre 3 e 20 caracteres.";
    public const string LengthMin8Max40 = "Este campo deve ter entre 8 e 40 caracteres.";
    public const string LengthMin10Max11 = "Este campo deve ter entre 10 e 11 caracteres.";
    public const string LengthMin14Max15 = "Este campo deve ter entre 14 e 15 caracteres.";
    public const string LengthMin4Max65 = "Este campo deve ter entre 4 e 65 caracteres.";
    public const string LengthMin4Max80 = "Este campo deve ter entre 4 e 80 caracteres.";
    public const string LengthMin4Max25 = "Este campo deve ter entre 4 e 25 caracteres.";
    public const string LengthMin4Max600 = "Este campo deve ter entre 4 e 600 caracteres.";
    
    public const string ExceededAttempst = "Você excedeu o número de tentativas.";
    
    public const string PriceRequired = "Informe o preço do produto.";
    
    public const string TitleRequired = "Título do ticket é obrigatório.";
    public const string TitleLengthMin4Max65 = "Título do ticket deve ter entre 4 e 65 caracteres.";
    
    public const string BodyRequired = "Corpo do ticket é obrigatório.";
    public const string BodyLengthMin10Max700 = "Corpo do ticket deve ter entre 10 e 700 caracteres.";
}