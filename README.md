# EasyEvents

**EasyEvents** é uma biblioteca leve e extensível para aplicações .NET que unifica o envio de **comandos**, **eventos** síncronos e **eventos assíncronos** (enfileirados). Seu foco é fornecer uma camada simples de **mediador** com suporte embutido para **eventos de fila processados em background**, usando `Channel<T>` com suporte a configurações `Bounded` e `Unbounded`.

---

## 🚀 Funcionalidades

- ✅ Suporte a **Command Handlers** com resposta tipada (`SendCommandAsync`)
- ✅ Suporte a **Event Handlers** síncronos (`PublishEventAsync`)
- ✅ Suporte a **eventos em fila** com background worker (`PublishQueueAsync`)
- ✅ Fila com comportamento configurável:
  - [`Bounded`](https://learn.microsoft.com/dotnet/api/system.threading.channels.boundedchanneloptions)
  - [`Unbounded`](https://learn.microsoft.com/dotnet/api/system.threading.channels.unboundedchanneloptions)
- ✅ Facilidade de uso com `IServiceCollection`
- ✅ Logging com `ILogger`

---

## 📦 Instalação

```bash
dotnet add package EasyEvents
```

## Ou via PackageReference:
```xml
<PackageReference Include="EasyEvents" Version="x.y.z" />
```


### ⚙️ Como usar
## 1. Registre o serviço no Startup.cs ou Program.cs:
# Simples injecao, configuracao default
```C#
services.AddEasyEvents();
```
## Customizacao da Queue
# escolha entre Bounded ou Unbounded, nunca os dois juntos!

# Bounded
```C#
services.AddEasyEvents(options => {
    option.QueueBounded = new BoundedChannelOptions(100) {
        SingleReader = true,
        SingleWriter = true,
        AllowSynchronousContinuations = false,
        FullMode = BoundedChannelFullMode.Wait
    };
});
```
# Unbounded 
```C#
services.AddEasyEvents(options => {    
    option.QueueUnbounded = new UnboundedChannelOptions {
        SingleReader = true,
        SingleWriter = true,
        AllowSynchronousContinuations = false
    };
});
```
### 🧑‍💻 Uso dos eventos e comandos

## 🔄 Enviar um comando (com resposta)

```C#
public class CreateOrderCommand : ICommand { ... }

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, Guid> {
    public Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken) {
        // lógica
    }
}


var result = await _easyEvents.SendCommandAsync<CreateOrderCommand, Guid>(command);

```


### 📢 Publicar evento (síncrono)
```C#
public class OrderCreatedEvent : IEvent { ... }

public class OrderCreatedHandler : IEventHandler<OrderCreatedEvent> {
    public Task HandleAsync(OrderCreatedEvent @event, CancellationToken cancellationToken) {
        // lógica
    }
}

await _easyEvents.PublishEventAsync(new OrderCreatedEvent(...));
```
### 🕒 Publicar evento em fila (assíncrono)

```C#
public class EmailNotificationEvent : IQueue { ... }

public class EmailNotificationHandler : IQueueHandler<EmailNotificationEvent> {
    public Task HandleAsync(EmailNotificationEvent queueEvent, CancellationToken cancellationToken) {
        // lógica assíncrona
    }
}

await _easyEvents.PublishQueueAsync(new EmailNotificationEvent(...));

```

### 🧪 Testes e Contribuição
# Contribuições são muito bem-vindas para tornar o EasyEvents ainda melhor!

Se você encontrou um bug, deseja sugerir melhorias ou adicionar novas funcionalidades, siga os passos abaixo:

Fork este repositório

Crie uma branch com um nome descritivo:
git checkout -b feature/nome-da-sua-feature

Faça suas alterações com commits claros e organizados

Push para o seu fork:
git push origin feature/nome-da-sua-feature

Abra um Pull Request explicando o que foi feito

Se possível, inclua testes que cubram a nova funcionalidade ou correção.



### 📄 Licença
Este projeto está licenciado sob a MIT License.