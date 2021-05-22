open System.Text
open System.Security.Cryptography

// Thanks to http://blog.stermon.com/articles/2016/10/09/fsharp-basic-merkle-tree.html
module Hashing =
    let byteToHex : byte -> string = fun b -> b.ToString("x2")

    let bytesToHex : byte array -> string =
        fun bytes ->
            bytes
            |> Array.fold (fun a x -> a + (byteToHex x)) ""

    let utf8ToBytes : string -> byte array = fun utf8 -> Encoding.UTF8.GetBytes utf8

    let sha256' : byte array -> byte array =
        fun bytes ->
            use sha256 = SHA256.Create()

            sha256.ComputeHash(buffer = bytes)

    (* mon@razerRamon:~$ echo -n 'foo' | sha256sum
     2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae  - *)
    let sha256 = utf8ToBytes >> sha256' >> bytesToHex

module Block =
    type Transaction =
        { sender: string
          recipient: string
          timestamp: int
          amount: int }

    type Block =
        { index: int
          timestamp: int
          transactions: Transaction array
          nonce: int
          previousHash: string
          hash: string }


[<EntryPoint>]
let main argv =
    let exampleTransaction : Block.Transaction =
        { sender = "gonza"
          recipient = "juli"
          timestamp = 0
          amount = 10 }

    let example : Block.Block =
        { index = 0
          timestamp = 0
          transactions = [| exampleTransaction |]
          nonce = 0
          previousHash = "asd"
          hash = "asd" }

    sprintf "%A" example
    |> fun x ->
        x |> printfn "%A"
        x
    |> Hashing.sha256
    |> printfn "%A"

    0 // return an integer exit code
