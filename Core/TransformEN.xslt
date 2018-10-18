<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
    <h1>Order #<xsl:value-of select="order/info/orderId" /></h1>
	<p>Date: <xsl:value-of select="order/info/date" /></p>
	<p>Paper type: <xsl:value-of select="order/info/paperType" /></p>
	<p>Crop mode: <xsl:value-of select="order/info/cropMode" /></p>
	<p>Order total: <xsl:value-of select="order/info/orderCost" /></p>
	<p>Total payment: <xsl:value-of select="order/info/totalCost" /></p>

	<h2>Customer info</h2>
	<p>Name: <xsl:value-of select="order/customer/name" /></p>
	<p>Phone: <xsl:value-of select="order/customer/phone" /></p>
	<p>Email: <xsl:value-of select="order/customer/email" /></p>

	<h2>Photos</h2>

	<table border="1">
		<tr>
			<th>Format</th>
			<th>Quantity</th>
			<th>Cost</th>
		</tr>
		<xsl:for-each select="order/formats/format">
			<xsl:if test="count(./photo) &gt; 0">
				<tr>
					<td><xsl:value-of select="@name" /></td>
					<td><xsl:value-of select="@count" /></td>
					<td><xsl:value-of select="@cost" /></td>
				</tr>
			</xsl:if>
		</xsl:for-each>
	</table>

	<xsl:if test="count(order/services/service) &gt; 0">
		<h2>Services</h2>

		<table border="1">
			<tr>
				<th>Service</th>
				<th>Cost</th>
			</tr>

			<xsl:for-each select="order/services/service">
				<tr>
					<td><xsl:value-of select="@name" /></td>
					<td><xsl:value-of select="@price" /></td>
				</tr>
			</xsl:for-each>
		</table>
	</xsl:if>
  </body>
  </html>
</xsl:template>
</xsl:stylesheet>