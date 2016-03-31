# JobLogger
JobLogger is a project built with the intention to refactoring and transform spaghetti code into well structured components with unit tests and samples.

##Logger
JobLogger has 3 built-in loggers.

###ConsoleLogger

    //1. Set up the Logger
    LogManager.Current.RegisterLogger(new ConsoleLogger(),LoggerLevel.All);

    //2. Use the logger
    LogManager.Current.LogError("Its an error");
    LogManager.Current.LogWarning("Its a warning");
    LogManager.Current.LogMessage("Its just a message");


###SqlLogger
    //1. Set up the Logger
    LogManager.Current.RegisterLogger(new SqlLogger());

    //2. Use the logger
    LogManager.Current.LogMessage("I'm a messae");
    LogManager.Current.LogError("I'm an error");
    LogManager.Current.LogWarning("I'm a warning");

###TextFileLogger
    //1. Set up the Logger
    LogManager.Current.RegisterLogger(new TextFileLogger());

    //2. Use the logger
    LogManager.Current.LogMessage("I'm a messae");
    LogManager.Current.LogError("I'm an error");
    LogManager.Current.LogWarning("I'm a warning");


##Using with an IoC Container
You can use the **LogManager.Current** instance like a singleton and register after you configure it with your IoC Container

or

You can use the **LogManager** class and instance it and register with your container because it has a public and default constructor. Alternatively, you can register ILogFormatter instance to override default behavior.

##Raise Exception when there is no Logger for this log level
Simply set **LogManager.Current.SuppressErrorOnNotFoundLogger**  to false (true is the default value).
