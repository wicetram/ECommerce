E-Commerce Payment Integration (Clean Architecture Case)

Bu proje Balance Management API ile e-ticaret ödeme akışını entegre eden örnek bir Clean Architecture uygulamasıdır.

Architecture
Domain → Order, OrderItem, Money (pure business rules)
Application → CQRS (MediatR), DTOs, Result pattern
Infrastructure → Balance API HttpClient + Polly, MemoryCache OrderRepository
WebAPI → REST endpoint’ler (products, create, complete)

Balance Management API
BaseUrl: https://balance-management-pi44.onrender.com

Kullanılan dış endpoint’ler:
• GET /api/products
• POST /api/balance/preorder
• POST /api/balance/complete

Bizim API → Bu uçlara proxy / orchestrator olarak bağlanır.

Bizim API Endpoint’leri
• GET /api/products → Ürünleri getirir
• POST /api/orders/create → Sipariş oluşturur + preorder çağrısını Balance API’ye atar
• POST /api/orders/{orderId}/complete → Siparişi tamamlar + complete çağrısını Balance API’ye atar

Akış Senaryosu
client GET /api/products ile ürünleri görür
client seçtiği ürün(ler) ile POST /api/orders/create atar
server MemoryCache içinde Order’ı oluşturur
server dış API’ye preorder gönderir ve bloklamayı yapar
client daha sonra POST /api/orders/{id}/complete atar
server dış API’ye complete gönderir
success geldiğinde Order status = completed yapılır

Teknik Noktalar
• DbContext yok, EF yok
• MemoryCache repository kullanıldı
