using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

    public interface IListener<T>
	{
		void Handle(T ev);
	}
    public interface IEmmiter<T>
    {
        T Emit();
    }


    public class Transmitter
	{
		public static event Action<object> OnMessageSend;
        public static event Action<object, MonoBehaviour> OnMessageReceive;


        public static void SendByObject(object ev)
        {

            if (ev == null)
            {
                throw new ArgumentNullException(nameof(ev));
            }

            Type eventType = ev.GetType();
            MethodInfo sendMethod = typeof(Transmitter).GetMethod(nameof(Send), BindingFlags.Public | BindingFlags.Static);
            MethodInfo genericSendMethod = sendMethod.MakeGenericMethod(eventType);
            genericSendMethod.Invoke(null, new[] { ev });
        }

        public static void Send<T>(T ev) 
		{
			if (ev != null)
			{
                OnMessageSend?.Invoke(ev);

                UnityEngine.MonoBehaviour[] allMonoBehavoir = UnityEngine.Object.FindObjectsOfType<UnityEngine.MonoBehaviour>();
				var all = UnityEngine.Object.FindObjectsOfType<UnityEngine.MonoBehaviour>().Where(i => i is IListener<T>).Select(i => i as IListener<T>).ToList();

				foreach (UnityEngine.MonoBehaviour objInScene in allMonoBehavoir)
				{
					IListener<T> eventListner = objInScene as IListener<T>;
					if (eventListner != null)
					{
                        OnMessageReceive?.Invoke(ev, objInScene);
                        eventListner.Handle(ev);
					}
				}
			}
		}

		public static void Emit<T>(IEmmiter<T> emitter)
		{
			Send(emitter.Emit());
		}

		public static T Ask<T>(IListener<T> forListener = null)
		{
			//Debug.LogError("Ask about type = " + typeof(T));
			UnityEngine.MonoBehaviour[] allMonoBehavoir =
				UnityEngine.Object.FindObjectsOfType<UnityEngine.MonoBehaviour>();
			var all = UnityEngine.Object.FindObjectsOfType<UnityEngine.MonoBehaviour>().Where(i => i is IEmmiter<T>)
				.Select(i => i as IEmmiter<T>).ToList();
			T responce = default(T);
			foreach (UnityEngine.MonoBehaviour objInScene in allMonoBehavoir)
			{
                IEmmiter<T> emiter = objInScene as IEmmiter<T>;
				if (emiter != null)
				{
					//Debug.LogError("Find for  type = " + typeof(T)+" exited emiter = "  + emiter.GetType());
					responce = emiter.Emit();
				}
			}
			if(forListener!=null)
				forListener.Handle(responce); 
			return responce;
		}
	}
