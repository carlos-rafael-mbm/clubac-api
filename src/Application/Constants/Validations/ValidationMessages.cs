namespace ClubApi.Application.Constants.Validations;

public class ValidationMessages
{
    public const string IS_REQUIRED = "El campo {0} es requerido";
    public const string MAXIMUM_LENGTH = "El campo {0} no puede exceder de {1} caracteres";
    public const string MINIMUM_LENGTH = "El campo {0} debe tener al menos {1} caracteres";
    public const string EMAIL_FORMAT = "El campo {0} no tiene un formato válido";
    public const string POSITIVE_INTEGER = "El campo {0} debe ser un entero positivo";
    public const string INTERNAL_SERVER_ERROR = "Ocurrió un error al realizar la operación";
    public const string NOT_FOUND = "{0} con ID {1} no encontrado";
    public const string INVALID_CREDENTIALS = "Credenciales inválidas";
    public const string INVALID_REFRESH_TOKEN = "Refres token inválido";
    public const string INVALID_EXPIRED_REFRESH_TOKEN = "Refres token inválido o expirado";
    public const string EXIT_ALREADY_REGISTERED = "La salida ya fue registrada";
}
