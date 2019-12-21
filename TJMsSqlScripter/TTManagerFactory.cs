using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{

  internal class TTManagerFactory
  {
    private static TTManager Manager { get; set; } = null;

    public static TTManager Create()
    {
      TTManager LocalManager = Manager;
      if (Manager == null)
      {
        LocalManager = new TTManager();
        CreateMembersOfTheClass(LocalManager);
        Manager = LocalManager;
      }
      return LocalManager;
    }

    private static void CreateMembersOfTheClass(TTManager manager)
    { // Здесь в нужном порядке создаются члены класса Manager ссылаясь один на другого при создании. Ссылка передаётся через аргумент конструктора. //
      manager.Connection = new TTConnection();
      //manager.Procedure = TTProcedureExecutor.Create(manager.Connection);
      //manager.ApplicationServer = TTApplicationServer.Create(manager.Procedure);
    }
  }
}
