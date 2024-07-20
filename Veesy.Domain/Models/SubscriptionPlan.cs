namespace Veesy.Domain.Models;

/// <summary>
/// Tabella identificativi dei vari abbonamenti proposti dall'applicazione. Inoltre, attraverso i parametri "AllowedMegaByte" e "AllowedMediaNumber", viene indicato le limitazioni su ciascun pacchetto.
/// <param name="Id">Identificativo univoco del record.</param>
/// <param name="Name">Nome del piano di abbonamento.</param>
/// <param name="Description">Descrizione del piano di abbonamento.</param>
/// <param name="Price">Prezzo del piano di abbonamento.</param>
/// <param name="AllowedMegaByte">Quantità massima di Megabyte che possono essere caricati sul server per Account.</param>
/// <param name="IsMediaFormatsInclude">Se true nel calcolo dei magabyte devono essere incluse anche i formati parlalleli.</param>
/// <param name="AllowedMediaNumber">Numero massimo di media che possono essere caricati sul server per Account.</param>
/// <param name="MyUsers">Lista virtuale di tutti gli utenti in possesso di quello specifico pacchetto.</param>
/// </summary>

public class SubscriptionPlan : TrackableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int AllowedMegaByte { get; set; }
    public int AllowedPortfolio { get; set; }
    public bool IsMediaFormatsInclude { get; set; } 
    public int AllowedMediaNumber { get; set; } 
    public virtual List<MyUserSubscriptionPlan> MyUserSubscriptionPlans { get; set; }
}