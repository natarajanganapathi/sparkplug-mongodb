namespace SparkPlug.Persistence.Abstractions;

public class PersistenceException : Exception { }
public class CreateEntityException : PersistenceException { }
public class DeleteEntityException : PersistenceException { }
public class GetEntityException : PersistenceException { }
public class UpdateEntityException : PersistenceException { }
