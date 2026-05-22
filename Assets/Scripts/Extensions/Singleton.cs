namespace Extensions {
    public class Singleton<T> where T: Singleton<T>, new() {
        
        //==================================================||Properties        
        public static T Instance => _instance ??= new T();
        
       //==================================================||Fields 
        private static T _instance;
    }
}