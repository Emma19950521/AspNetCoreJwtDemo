 <h1>AspNetCoreJwtDemo</h1>
<p>範例專案：ASP.NET Core 8 + JWT 認證 + Swagger + HTML 前端測試<br>
適合初學者練習 API 認證、角色驗證與前端呼叫流程</p>

<h2>專案功能</h2>
<ul>
    <li>註冊 API <code>/api/Auth/register</code> 建立帳號</li>
    <li>登入 API <code>/api/Auth/login</code> → 取得 JWT Token</li>
    <li>Profile API <code>/api/Auth/profile</code> → 受保護 API，可讀取 Claims</li>
    <li>Admin API <code>/api/Auth/admin</code> → 角色驗證 <code>[Authorize(Roles="Admin")]</code></li>
    <li>Swagger 支援 Bearer Token 測試</li>
    <li>HTML 前端頁面 <code>index.html</code> 可直接操作登入、呼叫 API</li>
</ul>

<h2> 安裝與執行</h2>
<ol>
<li>下載或 clone 專案：
    <pre><code>git clone https://github.com/Emma19950521/AspNetCoreJwtDemo.git
cd AspNetCoreJwtDemo</code></pre>
</li>
<li>安裝依賴套件：
    <pre><code>dotnet restore</code></pre>
</li>
<li>啟動專案：
    <pre><code>dotnet run</code></pre>
</li>
</ol>

<h2>JWT 認證測試流程</h2>
<h3>1. 註冊</h3>
<pre><code>POST /api/Auth/register
Content-Type: application/json
{
  "username": "123",
  "password": "123"
}</code></pre>>


<h3>2. 登入拿 Token</h3>
<pre><code>POST /api/Auth/login
Content-Type: application/json

{
"username": "123",
"password": "123",
"role": "Admin"
}</code></pre>

<p>Response:</p>
<pre><code>{
"token": "&lt;JWT_TOKEN&gt;"
}</code></pre>

<h3>3. 帶 Token 呼叫受保護 API</h3>
<p>在 Swagger 點選 <strong>Authorize</strong> → 貼上 <code>Bearer &lt;JWT_TOKEN&gt;</code></p>
<p>或使用 curl：</p>
<pre><code>curl -X GET "https://localhost:&lt;port&gt;/api/Auth/profile" -H "Authorization: Bearer &lt;JWT_TOKEN&gt;"</code></pre>




<h2>角色測試</h2>
<ul>
<li>Admin 用戶：<code>username: admin, password: 123, role: Admin</code></li>
<li>一般用戶：<code>username: user, password: 123, role: User</code></li>
</ul>
<p>
<code>[Authorize]</code> → 所有登入用戶都可進入<br>
<code>[Authorize(Roles="Admin")]</code> → 只有 Admin 可以進入
</p>

<h2>HTML 前端測試</h2>
<ul>
<li><code>index.html</code> 位於 <code>wwwroot</code> 目錄</li>
<li>可直接登入取得 token，呼叫 <code>/profile</code> 或 <code>/admin</code></li>
<li>結果顯示在網頁 <code>&lt;pre&gt;</code> 區塊</li>
</ul>

<h2> 建議改進</h2>
<ul>
<li>導入資料庫，取代靜態 List&lt;User&gt;</li>
<li>改成非對稱加密 RS256</li>
<li>Token 過期自動刷新 (Refresh Token)</li>
</ul>
