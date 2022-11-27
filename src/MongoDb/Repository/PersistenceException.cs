namespace SparkPlug.Persistence.Abstractions;

[Serializable]
public class PersistenceException : Exception { }

[Serializable]
public class CreateEntityException : PersistenceException { }

[Serializable]
public class DeleteEntityException : PersistenceException { }

[Serializable]
public class GetEntityException : PersistenceException { }

[Serializable]
public class UpdateEntityException : PersistenceException { }
