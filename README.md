Делал на основе этой статьи:
https://www.codemag.com/Article/2409071/Implementing-the-Outbox-Pattern-with-Kafka-and-C

docker compose сделал на основе этой статьи:
https://code-maze.com/aspnetcore-using-kafka-in-a-web-api/
https://stackblogger.com/kafka-cluster-using-docker/

запрос на создание заказа:
curl --location 'http://localhost:5014/api/Order/CreateOrder' \
--header 'Content-Type: application/json' \
--data '{
    "Order_Id": 5,
    "Customer_Id": 45,
    "Order_Date": "2024-06-05",
    "Amount": 4567
}'

создать из консоли topic:
docker exec -it broker kafka-topics --create --topic order-events --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1
docker exec -it broker kafka-topics --create --topic test --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1