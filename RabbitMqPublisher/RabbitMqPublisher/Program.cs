using System.Text;
using RabbitMQ.Client;

//Bağlantı Oluşturma
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

//Bağlantıyı Aktifleştirme ve Kanal Açma
using IConnection connection =  await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

//Queue oluşturma
await channel.QueueDeclareAsync(queue:"umut",exclusive:false,autoDelete: false,durable:true); // Mesajların sunucudaki bir sorun nedeniyle kaybolmasını önlemek için 
//durability parametresini true yapmalıyız

//Queue'ya Mesaj Gönderme
//Byte türünde mesaj kabul etmektedir. Bu yüzden byte'a dönüştürmemiz gerekmektedir.
var props = new BasicProperties(); //BasicProperties sınıfından yeni nesne oluştur
props.Persistent = true; //Parametresini true yap
byte [] message = Encoding.UTF8.GetBytes("Selam");  
await channel.BasicPublishAsync(exchange:"",routingKey:"umut",body:message, basicProperties:props, mandatory: true); //oluşturduğumuz props u uygun parametreye veririz
//Manndorty ise routing key de verilen isimle uygun bir kuyruk bulamazsa mesajın kaybolmasını engeller true yapılırsa

Console.Read();