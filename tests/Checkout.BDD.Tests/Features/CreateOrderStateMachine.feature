Feature: CreateOrderStateMachine

Saga State Machine Checkout

@tag1
Scenario: Authorize Payment When PreOrder Created
	Given CorrelationId "72786ccc-f02b-4925-a5c9-f0e216518b0c"
	And OrderId 123456
	When PreOrderCreated
	Then OrderRepository Has Order With Id 123456 And With Status "PreOrder"
	Then Consume PreOrderCreated
	Then Publish AuthorizePayment
	Then Saga current state is "AuthorizingPayment"
	Then Saga has same correlationId

Scenario: Process On Vendor When Payment Authorized
	Given CorrelationId "d76f7296-c4bd-4b53-a412-454bf0eebe12"
	And OrderId 7891011
	When PaymentAuthorized
	Then Consume PaymentAuthorized
	Then OrderRepository Has Order With Id 7891011 And With Status "PaymentAuthorized"
	Then Publish ProcessOnVendor
	Then Saga current state is "ProcessingOnVendor"
	Then Saga has same correlationId
	
Scenario: Confirm Order Payment When Vendor Processed
	Given CorrelationId "1edd0d7b-523c-4a6e-8398-e5da33b572d0"
	And OrderId 12131415
	When VendorProcessed
	Then Consume VendorProcessed
	Then OrderRepository Has Order With Id 12131415 And With Status "CreatedOnVendor"
	Then Publish ConfirmPayment
	Then Saga current state is "ConfirmingPayment"
	Then Saga has same correlationId

Scenario: Finalize Saga When Payment Confirmed
	Given CorrelationId "35dc7316-9788-410d-9bdc-d3d16288b412"
	And OrderId 115599879
	When PaymentConfirmed
	Then Consume PaymentConfirmed
	Then OrderRepository Has Order With Id 115599879 And With Status "PaymentConfirmed"
	Then Saga current state is "Final"
	Then Saga has same correlationId