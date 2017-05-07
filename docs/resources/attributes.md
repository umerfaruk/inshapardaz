Gets List of all attribute types used in application. This is a lookup resource and represent the attribute values returned in various resources.

## Url
`api/attributes`


### Request
```
GET /api/attributes HTTP/1.1
Accept: application/json
```

### Response
```
HTTP/1.1 200 OK
Content-Type: application/json
```

``` javascript
    [
        {
            key: "None",
            value: 0
        },{
            key: "Male",
            value: 1
        },{
            key: "Female",
            value: 16
        },{
            key: "Singular",
            value: 256
        },{
            key: "Plural",
            value: 4096
        },{
            key: "Ism",
            value: 65536
        },{
            key: "Sift",
            value: 131072
        },
        // Other values here
    ]
```

This value field  resource should be used as flag. Each bit represent a different type of attribute. For example a word can be used for female and make so both bis would be on for the value `000000011`;

## Details of Attributes Values

Attribute values are a 64-bit value with every bit representing a different property of word. These properties are divided into following groups:

### Gender

Represents gender for the word and uses first two bytes

| Description |       Value         |
|-------------|---------------------|
| None        | 0x0000000000000000  |
| Male        | 0x0000000000000001  |
| Female      | 0x0000000000000010  |
| Both        | 0x0000000000000010  |

### Multiplicity

Represents if the word is used in singular or plural context. 3-4 bits carry the value

| Description |       Value         |
|-------------|---------------------|
| None        | 0x0000000000000000  |
| Singular    | 0x0000000000000100  |
| Plural      | 0x0000000000001000  |
| Both        | 0x0000000000001100  |

### Grammatical Type

Represents the grammatical type of urdu word 

| Description |       Value         |
|-------------|---------------------|
| Ism         |  0x0000000000010000 |
| Sift        |  0x0000000000020000 |
| Feal        |  0x0000000000030000 |
| Harf        |  0x0000000000040000 | 

### Grammatical SubType

Represent grammatical subtype of urdu word

| Description   |       Value         |
|---------------|---------------------|
| IsmNakra           | 0x0000000000110000  | 
| IsmAla             | 0x0000000001110000  |
| IsmSoot            | 0x0000000002110000  | 
| IsmTashgir         | 0x0000000003110000  | 
| IsmMasghar         | 0x0000000004110000  | 
| IsmMukabbar        | 0x0000000005110000  | 
| IsmZarf            | 0x0000000006110000  | 
| IsmZarfMakan       | 0x0000000016110000  | 
| IsmZarfZaman       | 0x0000000026110000  |
| IsmJama            | 0x0000000036110000  |
| IsmMuarfa          | 0x0000000000210000  | 
| IsmAlam            | 0x0000000001210000  | 
| Khitaab            | 0x0000000011210000  |
| Lakab              | 0x0000000021210000  |
| Takhallus          | 0x0000000031210000  |
| Kunniyat           | 0x0000000041210000  |
| Urf                | 0x0000000051210000  |
| IsmZameer          | 0x0000000000310000  | 
| ZameerShakhsi      | 0x0000000001310000  | 
| Ghaib              | 0x0000000011310000  | 
| Hazir              | 0x0000000021310000  | 
| Mutakallam         | 0x0000000031310000  | 
| Mukhatib           | 0x0000000041310000  | 
| ZameerIshara       | 0x0000000002310000  | 
| ZameerIsharaKareeb | 0x0000000012310000  |
| ZameerIsharaBaeed  | 0x0000000022310000  |
| ZameerIstafham     | 0x0000000003310000  | 
| ZameerMosula       | 0x0000000004310000  | 
| ZameerTankeer      | 0x0000000005310000  |
| IsmIshara          | 0x0000000000410000  | 
| IsmIsharaKareeb    | 0x0000000001410000  | 
| IsmIsharaBaeed     | 0x0000000002410000  |
| IsmMosool          | 0x0000000000510000  |
| IsmMujarrad        | 0x0000000000310000  | 
| IsmKaifiat         | 0x0000000000410000  | 
| IsmHasilMasdar     | 0x0000000000510000  | 
| IsmJamid           | 0x0000000000610000  | 
| IsmMaada           | 0x0000000000710000  | 
| IsmAddad           | 0x0000000000810000  | 
| IsmMuawqza         | 0x0000000000910000  |
| IsmTashgeer        | 0x0000000000A10000  |
| SiftZati           | 0x0000000000120000  |
| SiftNisbati        | 0x0000000000220000  |
| SiftAdadi          | 0x0000000000320000  | 
| SiftMiqdari        | 0x0000000000420000  | 
| SiftIshara         | 0x0000000000520000  |
| SiftMushba         | 0x0000000000620000  |
| FealMutaddi        | 0x0000000000130000  |
| FealLazim          | 0x0000000000230000  |
| FealNakis          | 0x0000000000330000  | 
| FealImdadi         | 0x0000000000430000  |
| FealTaam           | 0x0000000000530000  |
| MutaliqFeal        | 0x0000000000630000  |
| HarfFijaia         | 0x0000000000140000  |
| HarfJaar           | 0x0000000000240000  |
| HarfNafi           | 0x0000000000340000  |
| HarfDuaiya         | 0x0000000000440000  |
| HarfTashbih        | 0x0000000000540000  |
| HarfTanbeeh        | 0x0000000000640000  |
| HarfTahseen        | 0x0000000000740000  |
| HarfIstasna        | 0x0000000000840000  |
| HarfShart          | 0x0000000000940000  |
| HarfTaajub         | 0x0000000000A40000  |
| HarfNidaaiya       | 0x0000000000B40000  |
| HarfRabt           | 0x0000000000C40000  |
| HarfQasam          | 0x0000000000D40000  |
| HarfIstasjab       | 0x0000000000E40000  |
| HarfIstafham       | 0x0000000000F40000  |
| HarfTabeh          | 0x0000000001040000  |
| HarfEjab           | 0x0000000001140000  |
| HarfJaza           | 0x0000000001240000  |
| HarfTahajji        | 0x0000000001340000  |
| HarfSoot           | 0x0000000001440000  |
| HarfIzafat         | 0x0000000001540000  |
| HarfNida           | 0x0000000001640000  |
| HarfTardeed        | 0x0000000001740000  |
| HarfTakeed         | 0x0000000001840000  |
| HarfAtaf           | 0x0000000001940000  |
| HarfTanaffar       | 0x0000000001A40000  |
| HarfBiyaniya       | 0x0000000001B40000  |
| HarfIstadraak      | 0x0000000001C40000  |
| HarfIllat          | 0x0000000001D40000  |
| HarfTakhsees       | 0x0000000001E40000  |
| HarfTamanna        | 0x0000000001F40000  |