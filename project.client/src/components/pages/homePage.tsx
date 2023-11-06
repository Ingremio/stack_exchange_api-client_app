import { Card, CardBody, CardImg, CardSubtitle, CardTitle, Col, Container, List, Row } from "reactstrap";
import Ufo from "./../../../public/ufo.svg";

export default function HomePage() {
    return (
        <Container className="mt-5">
            <Row>
                <Col sm="12" md={{ size: 6, offset: 3 }}>
                    <Card>
                        <CardBody>
                            <CardImg className="mb-3 logo" src={Ufo.toString()} alt="Ufo image cap" width={256} height={256}>
                            </CardImg>
                            <CardTitle tag="h5">
                                You're logged in, great!
                            </CardTitle>
                            <CardSubtitle className="mb-2 text-muted" tag="h6" >
                                This app is built with:
                            </CardSubtitle>
                            <List type="unstyled">
                                <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
                                <li><a href='https://vitejs.dev/'>Vite</a> for next generation frontend tooling, with fast server start and updates</li>
                                <li><a href='https://facebook.github.io/react/'>React</a> and <a href='https://www.typescriptlang.org/'>TypeScript</a> for client-side code with type syntax</li>
                                <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
                            </List>
                        </CardBody>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
}