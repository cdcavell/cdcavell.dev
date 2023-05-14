<?xml version="1.0" encoding="iso-8859-1"?>

<!-- ********************************************************************************* -->
<!-- | Revisions:                                                                    | -->
<!-- |*******************************************************************************| -->
<!-- | Christopher D. Cavell  | 0.0.0.1 | 12/11/2021 | Initial Development           | -->
<!-- ********************************************************************************* -->

<xsl:stylesheet version='1.0' xmlns:h="http://www.w3.org/1999/xhtml" xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>

	<xsl:output method='text' encoding='utf-8'/>

	<xsl:variable name='newline'>
		<xsl:text>
</xsl:text>
	</xsl:variable>
	<xsl:variable name ="Dot">
		<xsl:text>.</xsl:text>
	</xsl:variable>
	<xsl:variable name ="Dash">
		<xsl:text> - </xsl:text>
	</xsl:variable>
	<xsl:variable name ="Table">
		<xsl:text>|</xsl:text>
	</xsl:variable>
	<xsl:variable name ="TableEmpty">
		<xsl:text> |</xsl:text>
	</xsl:variable>
	<xsl:variable name ="Divider">
		<xsl:value-of select="$newline"/>( [Home](Home) )<xsl:value-of select="$newline"/><xsl:value-of select="$newline"/>
	</xsl:variable>
	<xsl:variable name ="H1">
		<xsl:text># </xsl:text>
	</xsl:variable>
	<xsl:variable name ="H2">
		<xsl:text>## </xsl:text>
	</xsl:variable>
	<xsl:variable name ="H3">
		<xsl:text>### </xsl:text>
	</xsl:variable>
	<xsl:variable name ="H4">
		<xsl:text>#### </xsl:text>
	</xsl:variable>

	<xsl:template match="text()" />

	<xsl:template match="/">
		<!-- xsl:apply-templates select="//assembly"/ -->
		<xsl:apply-templates select="//member[contains(@name,'T:')]"/>
	</xsl:template>

	<xsl:template match="assembly">
		<xsl:value-of select="$newline"/>
		<xsl:value-of select="$H1"/>
		<xsl:value-of select="name"/>
	</xsl:template>
	<!-- Class -->
	<xsl:template match="//member[contains(@name,'T:')]">
		<xsl:variable name ="SearchString" select="substring-after(@name, 'T:')" />
		<xsl:variable name="Class" select="class"/>
		<xsl:variable name ="ClassName">
			<xsl:choose>
				<xsl:when test="$Class != ''">
					<xsl:for-each select="class">
						<xsl:value-of select="$Class"/>
					</xsl:for-each>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="ClassLoop">
						<xsl:with-param name="parameter">
							<xsl:value-of select="$SearchString"/>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name ="PackageName" select="substring-before($SearchString, concat('.', $ClassName))" />
		<xsl:value-of select="$newline"/>&lt;a name='<xsl:value-of select="$PackageName"/><xsl:value-of select="$Dot"/><xsl:value-of select="$ClassName"/>'&gt;&lt;/a&gt;
		<xsl:value-of select="$newline"/><xsl:value-of select="$H2"/><xsl:value-of select="$PackageName"/><xsl:value-of select="$Dot"/><xsl:value-of select="$ClassName"/>
		<!-- Summary -->
		<xsl:value-of select="$newline"/><xsl:value-of select="normalize-space(summary)" /><xsl:value-of select="$newline"/><xsl:value-of select="$newline"/>
		<!-- Example -->
		<xsl:if test="count(example)!=0">
			<xsl:value-of select="$newline"/>
			<xsl:value-of select="$H4"/>
			<xsl:text>Example:</xsl:text>
			<xsl:value-of select="$newline"/>
			<xsl:for-each select="example">
				```
				<xsl:value-of select="current()" />
				```
			</xsl:for-each>
		</xsl:if>
		<!-- Revision -->
		<xsl:call-template name="AddNewLine">
			<xsl:with-param name="text">
				<xsl:value-of select="normalize-space(revision)"/>
			</xsl:with-param>
		</xsl:call-template>
		<xsl:value-of select="$newline"/><xsl:value-of select="$newline"/>
		<!-- Fields -->
		<xsl:if test="//member[contains(@name,concat('F:',$SearchString))]">
			<xsl:value-of select="$Table"/>
			<xsl:text>Fields</xsl:text>
			<xsl:value-of select="$Table"/>
			<xsl:value-of select="$TableEmpty"/>
			<xsl:value-of select="$newline"/>
			<xsl:value-of select="$Table"/>
			<xsl:value-of select="$Dash"/>
			<xsl:value-of select="$Table"/>
			<xsl:value-of select="$Dash"/>
			<xsl:value-of select="$Table"/>
			<xsl:value-of select="$newline"/>
			<xsl:for-each select="//member[contains(@name,concat('F:',$SearchString))]">
				<xsl:variable name ="Member" select="substring-after(@name, concat('F:',$SearchString,'.'))" />
				<xsl:value-of select="$Table"/>
				<xsl:value-of select="$Member"/>
				<xsl:value-of select="$Table"/>
				<xsl:value-of select="normalize-space(value)"/>
				<xsl:value-of select="$Table"/>
				<xsl:value-of select="$newline"/>
			</xsl:for-each>
			<xsl:value-of select="$newline"/>
		</xsl:if>
		<!-- Properties -->
		<xsl:if test="//member[contains(@name,concat('P:',$SearchString))]">
			<xsl:value-of select="$Table"/>
			<xsl:text>Properties</xsl:text>
			<xsl:value-of select="$Table"/>
			<xsl:value-of select="$TableEmpty"/>
			<xsl:value-of select="$newline"/>
			<xsl:value-of select="$Table"/>
			<xsl:value-of select="$Dash"/>
			<xsl:value-of select="$Table"/>
			<xsl:value-of select="$Dash"/>
			<xsl:value-of select="$Table"/>
			<xsl:value-of select="$newline"/>
			<xsl:for-each select="//member[contains(@name,concat('P:',$SearchString))]">
				<xsl:variable name ="Member" select="substring-after(@name, concat('P:',$SearchString,'.'))" />
				<xsl:value-of select="$Table"/>
				<xsl:value-of select="$Member"/>
				<xsl:value-of select="$Table"/>
				<xsl:value-of select="normalize-space(value)"/>
				<xsl:value-of select="$Table"/>
				<xsl:value-of select="$newline"/>
			</xsl:for-each>
			<xsl:value-of select="$newline"/>
		</xsl:if>
		<!-- Methods -->
		<xsl:if test="//member[contains(@name,concat('M:',$SearchString))]">
			<xsl:value-of select="$H3"/>
			<xsl:text>Methods:</xsl:text>
			<xsl:value-of select="$newline"/>
			<xsl:for-each select="//member[contains(@name,concat('M:',$SearchString))]">
				<xsl:variable name ="Member" select="substring-after(@name, concat('M:',$SearchString,'.'))" />
				<xsl:variable name="Method" select="method"/>

				<xsl:choose>
					<xsl:when test="$Method != ''">
						<xsl:for-each select="method">
							<xsl:value-of select="$H4"/>
							<xsl:value-of select="normalize-space(current())"/>
							<xsl:value-of select="$newline"/>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$H4"/>
						<xsl:value-of select="$Member"/>
						<xsl:value-of select="$newline"/>
					</xsl:otherwise>
				</xsl:choose>

				<xsl:value-of select="$newline"/>
				<xsl:value-of select="normalize-space(summary)" />
				<xsl:value-of select="$newline"/>
				<!-- Parameters -->
				<xsl:if test="count(param)!=0">
					<xsl:value-of select="$newline"/>
					<xsl:value-of select="$Table"/>
					<xsl:text>Parameters</xsl:text>
					<xsl:value-of select="$Table"/>
					<xsl:value-of select="$TableEmpty"/>
					<xsl:value-of select="$newline"/>
					<xsl:value-of select="$Table"/>
					<xsl:value-of select="$Dash"/>
					<xsl:value-of select="$Table"/>
					<xsl:value-of select="$Dash"/>
					<xsl:value-of select="$Table"/>
					<xsl:value-of select="$newline"/>
					<xsl:for-each select="param">
						<xsl:variable name ="ParamValue" select="normalize-space(current())" />
						<xsl:value-of select="$Table"/>
						<xsl:value-of select="@name"/>
						<xsl:value-of select="$Table"/>
						<xsl:value-of select="$ParamValue"/>
						<xsl:value-of select="$Table"/>
						<xsl:value-of select="$newline"/>
					</xsl:for-each>
				</xsl:if>
				<!-- Returns -->
				<xsl:if test="count(returns)!=0">
					<xsl:value-of select="$newline"/>
					<xsl:value-of select="$H4"/>
					<xsl:text>Returns:</xsl:text>
					<xsl:value-of select="$newline"/>
					<xsl:for-each select="returns">
						<xsl:value-of select="normalize-space(current())"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="$newline"/>
					</xsl:for-each>
				</xsl:if>
				<!-- Exception -->
				<xsl:if test="count(exception)!=0">
					<xsl:value-of select="$newline"/>
					<xsl:value-of select="$H4"/>
					<xsl:text>Exceptions:</xsl:text>
					<xsl:value-of select="$newline"/>
					<xsl:for-each select="exception">
						<xsl:value-of select="normalize-space(substring-after(@cref, 'T:'))"/> ( <xsl:value-of select="normalize-space(current())"/> )<xsl:value-of select="$newline"/>
					</xsl:for-each>
				</xsl:if>
				<!-- Example -->
				<xsl:if test="count(example)!=0">
					<xsl:value-of select="$newline"/>
					<xsl:value-of select="$H4"/>
					<xsl:text>Example:</xsl:text>
					<xsl:value-of select="$newline"/>
					<xsl:for-each select="example">
						```C#
						<xsl:value-of select="current()" />
						```
					</xsl:for-each>
				</xsl:if>

				<xsl:value-of select="$H2" />
				<xsl:value-of select="$newline"/>
			</xsl:for-each>
		</xsl:if>

		<!-- End -->
		<xsl:value-of select="$Divider"/>
	</xsl:template>

	<xsl:template name="ClassLoop">
		<xsl:param name="parameter"></xsl:param>
		<xsl:choose>
			<xsl:when test="contains($parameter, '.')">
				<xsl:variable name="newparm" select="substring-after($parameter, '.')"/>
				<xsl:call-template name="ClassLoop">
					<xsl:with-param name="parameter">
						<xsl:value-of select="$newparm"/>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$parameter"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="AddNewLine">
		<xsl:param name="text"/>
		<xsl:param name="searchString">~</xsl:param>
		<xsl:choose>
			<xsl:when test="contains($text,$searchString)">
				<xsl:value-of select="normalize-space(substring-before($text,$searchString))"/>
				<xsl:value-of select="$newline"/>
				<!--  recursive call -->
				<xsl:call-template name="AddNewLine">
					<xsl:with-param name="text" select="substring-after($text,$searchString)"/>
					<xsl:with-param name="searchString" select="$searchString"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="normalize-space($text)"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
