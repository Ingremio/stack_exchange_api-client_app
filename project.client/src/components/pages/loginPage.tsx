import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { Button, Card, CardBody, CardImg, CardSubtitle, CardText, CardTitle, Col, Container, Form, Row } from "reactstrap";
import history from "../history";
import { userActions } from "../reducers/usersSlice";
import { useAppDispatch, useAppSelector } from "../storage";
import Ufo from "./../../../public/ufo.svg";

export default function LoginPage() {
    const dispatch = useAppDispatch();
    const user = useAppSelector(x => x.userState.user);
    const { handleSubmit } = useForm();

    useEffect(() => {
        if (user) {
            history.navigate?.('/');
        }

        dispatch(userActions.login())
    }, [user, dispatch]);
    
    function onSubmit() {
        window.location.href = "/oauth/login";
    }

    return (
        <Container className="mt-5">
            <Row>
                <Col sm="12" md={{ size: 6, offset: 3 }}>
                    <Card>
                        <CardBody>
                            <Form onSubmit={handleSubmit(onSubmit)} >
                                <CardImg className="mb-3 logo" src={Ufo.toString()} alt="Ufo image cap" width={256} height={256}>
                                </CardImg>
                                <CardTitle tag="h5">
                                    Welcome in The S-Files
                                </CardTitle>
                                <CardSubtitle className="mb-2 text-muted" tag="h6" >
                                    Stack Exchange API Client App
                                </CardSubtitle>
                                <CardText>
                                    To get started, click the button below and log in to your StackOverflow account.
                                    After successfully logging in, you will be able to start working. Good luck.
                                </CardText>
                                <Button >
                                    {'Take me there >'}
                                </Button>
                            </Form>
                        </CardBody>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
}