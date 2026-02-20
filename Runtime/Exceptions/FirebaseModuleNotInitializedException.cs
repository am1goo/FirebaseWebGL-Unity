using System;

public sealed class FirebaseModuleNotInitializedException : Exception
{
    public FirebaseModuleNotInitializedException(Type moduleType) : this(moduleType.Name)
    {

    }

    public FirebaseModuleNotInitializedException(string moduleName) : base($"firebase module '{moduleName}' is not initialzed")
    {

    }
}